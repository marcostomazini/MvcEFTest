using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using MvcEFTest.Entities;
using MvcEFTest.Models;
using MvcEFTest.Views;
using Xunit;

namespace MvcEFTest.Tests.Entities
{
    public sealed class MvcEFContextTest : IDisposable
    {
        private const int TestUserId = 1;

        private static readonly UserViewModel _unmodifiedUserViewModelWithModifiedChildren = GetUserViewModel(false, true);
        private static readonly UserViewModel _modifiedUserViewModelWithModifiedChildren = GetUserViewModel(true, true);

        private static readonly UserViewModel _unmodifiedUserViewModelWithUnmodifiedChildren = GetUserViewModel(false, false);
        private static readonly UserViewModel _modifiedUserViewModelWithUnmodifiedChildren = GetUserViewModel(false, false);

        private readonly MvcEFTestContext _context;

        public MvcEFContextTest()
        {
            _context = new MvcEFTestContext();
            Database.SetInitializer(new MvcEFDropCreateAlwaysInitializer());

            Mapper.CreateMap<UserViewModel, User>();
            Mapper.CreateMap<PhoneViewModel, Phone>();
        }
            
        [Fact]
        public void Users_ModificationsToNonTracking_WillNotAffectTrackedEntity()
        {
            var user = _context.Users.Find(TestUserId);
            var user2 = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
            user2.Name += "Abc";

            Assert.Equal(EntityState.Unchanged, _context.Entry(user).State);
            Assert.Equal(EntityState.Detached, _context.Entry(user2).State);

            Assert.NotEqual(user.Name, user2.Name);
        }

        [Fact]
        public void Users_ModificationsToNonTrackedChildren_WillNotAffectTrackedEntity()
        {
            var user = _context.Users.Find(TestUserId);
            var user2 = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

            user2.Phones.ToList()[0].Name += "Test";

            Assert.Equal(EntityState.Unchanged, _context.Entry(user).State);
            Assert.Equal(EntityState.Detached, _context.Entry(user2).State);

            Assert.Equal(user.Phones.Count, user2.Phones.Count);
            Assert.NotEqual(user.Phones.ToList()[0].Name, user2.Phones.ToList()[0].Name);
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_EntityStateRemainsUnchangedIfNotModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 5" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.Find(TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Unchanged, _context.Entry(model).State);
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_EntityStateModifiedIfModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex Version2",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 5" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.Find(TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Modified, _context.Entry(model).State);
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_ChildrenEntityStateBecomesAddedIfMapped()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.Find(TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Unchanged, _context.Entry(model).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfNotModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 5" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Detached, _context.Entry(model).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex123",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 5" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Detached, _context.Entry(model).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfChildrenModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Equal(EntityState.Detached, _context.Entry(model).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Detached, _context.Entry(model.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_AttachModifiedChildren_EntityAndChildrenUnchanged()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            Assert.DoesNotThrow(() => _context.Users.Attach(model));
            Assert.Equal(EntityState.Unchanged, _context.Entry(model).State);
            Assert.Equal(EntityState.Modified, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Unchanged, _context.Entry(model.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_AttachModifiedChildrenAndAnotherTrackedEntityExists_Throws()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
                Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                }
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
            var model2 = _context.Users.Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            Assert.Throws<InvalidOperationException>(() => _context.Users.Attach(model));
        }

        [Fact]
        public void UsersAsNoTracking_ManuallyAddChildrenAndAnotherTrackedEntityExists_DoesNotThrow()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex"
                //Phones = new List<PhoneViewModel>
                //{
                //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                //}
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
            var model2 = _context.Users.Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            model.Phones.Add(new Phone { Name = "Nexus 6" });
            model.Phones.Add(new Phone { Name = "Galaxy S3" });


            Assert.DoesNotThrow(() => _context.Users.Add(model));
            Assert.Equal(EntityState.Added, _context.Entry(model).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[1]).State);

            Assert.Equal(EntityState.Unchanged, _context.Entry(model2).State);
            Assert.Equal(EntityState.Unchanged, _context.Entry(model2.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Unchanged, _context.Entry(model2.Phones.ToList()[1]).State);
        }

        [Fact]
        public void UsersAsNoTracking_ManuallyAddChildrenAndAnotherModifiedTrackedEntityExists_OldEntityAddedNewEntityModified()
        {
            var viewModel = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex"
                //Phones = new List<PhoneViewModel>
                //{
                //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                //}
            };

            var model = _context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
            var model2 = _context.Users.Single(u => u.Id == TestUserId);

            Mapper.Map(viewModel, model);

            foreach (var phone in model2.Phones.ToList())
            {
                _context.Entry(phone).State = EntityState.Detached;
                model.Phones.Add(phone);
            }

            model2.Name = "abc";

            Assert.DoesNotThrow(() => _context.Users.Add(model));
            Assert.Equal(EntityState.Added, _context.Entry(model).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Added, _context.Entry(model.Phones.ToList()[1]).State);

            Assert.Equal(EntityState.Modified, _context.Entry(model2).State);
            Assert.Equal(EntityState.Unchanged, _context.Entry(model2.Phones.ToList()[0]).State);
            Assert.Equal(EntityState.Unchanged, _context.Entry(model2.Phones.ToList()[1]).State);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        private static IEnumerable<PhoneViewModel> GetPhoneViewModels(bool modified)
        {
            var unmodifiedPhones = new List<PhoneViewModel>
            {
                new PhoneViewModel { Id = 1, Name = "Nexus 5" },
                new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
            };

            var modifiedPhones = new List<PhoneViewModel>
            {
                new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
            };

            return modified ? modifiedPhones : unmodifiedPhones;
        }

        private static UserViewModel GetUserViewModel(bool modified, bool childrenModified)
        {
            var unmodifiedUser = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex",
            };

            var modifiedUser = new UserViewModel
            {
                Id = TestUserId,
                Name = "Rex234",
            };

            unmodifiedUser.Phones = GetPhoneViewModels(childrenModified);
            modifiedUser.Phones = GetPhoneViewModels(childrenModified);

            return modified ? modifiedUser : unmodifiedUser;
        }
    }
}
