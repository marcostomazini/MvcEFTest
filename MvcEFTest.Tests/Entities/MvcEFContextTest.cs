using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MvcEFTest.Entities;
using MvcEFTest.Models;
using MvcEFTest.ValueResolvers;
using MvcEFTest.Views;
using Xunit;

namespace MvcEFTest.Tests.Entities
{
    public sealed class MvcEFContextTest
    {
        private const int TestUserId = 1;

        private static readonly UserViewModel _unmodifiedUserViewModelWithModifiedChildren = GetUserViewModel(false, true);
        private static readonly UserViewModel _modifiedUserViewModelWithModifiedChildren = GetUserViewModel(true, true);

        private static readonly UserViewModel _unmodifiedUserViewModelWithUnmodifiedChildren = GetUserViewModel(false, false);
        private static readonly UserViewModel _modifiedUserViewModelWithUnmodifiedChildren = GetUserViewModel(true, false);

        public MvcEFContextTest()
        {
            Mapper.CreateMap<UserViewModel, User>()
                  .ForMember(m => m.Phones,
                             opt =>
                             opt.ResolveUsing(
                                 new EntityCollectionValueResolver<UserViewModel, PhoneViewModel, Phone>(vm => vm.Phones)))
                  .ReverseMap();
            Mapper.CreateMap<PhoneViewModel, Phone>().ReverseMap();
        }
            
        [Fact]
        public void Users_ModificationsToNonTracking_WillNotAffectTrackedEntity()
        {
            using (var context = new MvcEFTestContext())
            {
                var user = context.Users.Find(TestUserId);
                var user2 = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                user2.Name += "Abc";

                Assert.Equal(EntityState.Unchanged, context.Entry(user).State);
                Assert.Equal(EntityState.Detached, context.Entry(user2).State);

                Assert.NotEqual(user.Name, user2.Name);
            }
        }

        [Fact]
        public void Users_ModificationsToNonTrackedChildren_WillNotAffectTrackedEntity()
        {
            using (var context = new MvcEFTestContext())
            {
                var user = context.Users.Find(TestUserId);
                var user2 = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

                user2.Phones.ToList()[0].Name += "Test";

                Assert.Equal(EntityState.Unchanged, context.Entry(user).State);
                Assert.Equal(EntityState.Detached, context.Entry(user2).State);

                Assert.Equal(user.Phones.Count, user2.Phones.Count);
                Assert.NotEqual(user.Phones.ToList()[0].Name, user2.Phones.ToList()[0].Name);
            }
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_EntityStateRemainsUnchangedIfNotModified()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithUnmodifiedChildren;

                var model = context.Users.Find(TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Unchanged, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_ParentModifiedChildrenAdded()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _modifiedUserViewModelWithUnmodifiedChildren;

                var model = context.Users.Find(TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Modified, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_ChildrenEntityStateBecomesAddedIfMapped()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithModifiedChildren;

                var model = context.Users.Find(TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Unchanged, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Modified, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void Users_MappingFromViewModelToModel_ParentEntityModifiedAndChildrenAdded()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _modifiedUserViewModelWithModifiedChildren;

                var model = context.Users.Find(TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Modified, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Modified, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfNotModified()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithUnmodifiedChildren;

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Detached, context.Entry(model).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfModified()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _modifiedUserViewModelWithUnmodifiedChildren;

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Detached, context.Entry(model).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_MappingFromViewModelToModel_EntityStateDetachedIfChildrenModified()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithModifiedChildren;

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Equal(EntityState.Detached, context.Entry(model).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Detached, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_AttachModifiedChildren_EntityAndChildrenUnchanged()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithModifiedChildren;

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                // Reattaching equals reloading the whole entity from db?
                Assert.DoesNotThrow(() => context.Users.Attach(model));
                Assert.Equal(EntityState.Unchanged, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_AttachModifiedChildrenAndAnotherTrackedEntityExists_Throws()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = _unmodifiedUserViewModelWithModifiedChildren;

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                var model2 = context.Users.Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                Assert.Throws<InvalidOperationException>(() => context.Users.Attach(model));
            }
        }

        [Fact]
        public void UsersAsNoTracking_ManuallyAddChildrenAndAnotherTrackedEntityExists_DoesNotThrow()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = new UserViewModel
                {
                    Id = TestUserId,
                    Name = "Foo"
                    //Phones = new List<PhoneViewModel>
                    //{
                    //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                    //}
                };

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                var model2 = context.Users.Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                model.Phones.Add(new Phone { Name = "Nexus 6" });
                model.Phones.Add(new Phone { Name = "Galaxy S3" });


                Assert.DoesNotThrow(() => context.Users.Add(model));
                Assert.Equal(EntityState.Added, context.Entry(model).State);
                Assert.Equal(EntityState.Added, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Added, context.Entry(model.Phones.ToList()[1]).State);

                Assert.Equal(EntityState.Unchanged, context.Entry(model2).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_ManuallyAddAttachedChildrenAndAnotherModifiedTrackedEntityExists_ChildrenUnchanged()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = new UserViewModel
                {
                    Id = TestUserId,
                    Name = "Foo"
                    //Phones = new List<PhoneViewModel>
                    //{
                    //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                    //}
                };

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                var model2 = context.Users.Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                foreach (var phone in model2.Phones.ToList())
                {
                    // Deliberately commenting the next line, children are still in Unchanged EntityState
                    //_context.Entry(phone).State = EntityState.Detached;
                    model.Phones.Add(phone);
                }

                model2.Name = "abc";

                Assert.DoesNotThrow(() => context.Users.Add(model));
                Assert.Equal(EntityState.Added, context.Entry(model).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model.Phones.ToList()[1]).State);


                Assert.Equal(EntityState.Modified, context.Entry(model2).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[1]).State);
            }
        }


        [Fact]
        public void UsersAsNoTracking_ManuallyAddDetachedChildrenAndAnotherModifiedTrackedEntityExists_ParentCloned()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = new UserViewModel
                {
                    Id = TestUserId,
                    Name = "Foo"
                    //Phones = new List<PhoneViewModel>
                    //{
                    //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                    //}
                };

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                var model2 = context.Users.Single(u => u.Id == TestUserId);

                Mapper.Map(viewModel, model);

                foreach (var phone in model2.Phones.ToList())
                {
                    // Must detach the entity from the original user
                    context.Entry(phone).State = EntityState.Detached;
                    model.Phones.Add(phone);
                }

                model2.Name = "abc";

                Assert.DoesNotThrow(() => context.Users.Add(model));
                Assert.Equal(EntityState.Added, context.Entry(model).State);
                Assert.Equal(EntityState.Added, context.Entry(model.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Added, context.Entry(model.Phones.ToList()[1]).State);

                Assert.Equal(EntityState.Modified, context.Entry(model2).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[0]).State);
                Assert.Equal(EntityState.Unchanged, context.Entry(model2.Phones.ToList()[1]).State);
            }
        }

        [Fact]
        public void UsersAsNoTracking_ManuallyAddDetachedChildrenAndModifyTrackedEntity_SaveSuccessfully()
        {
            using (var context = new MvcEFTestContext())
            {
                var viewModel = new UserViewModel
                {
                    Id = TestUserId,
                    Name = "Foo"
                    //Phones = new List<PhoneViewModel>
                    //{
                    //    new PhoneViewModel { Id = 1, Name = "Nexus 6" },
                    //    new PhoneViewModel { Id = 2, Name = "Galaxy S3" }
                    //}
                };

                var model = context.Users.AsNoTracking().Single(u => u.Id == TestUserId);
                var model2 = context.Users.Single(u => u.Id == TestUserId);

                var originalUserCount = context.Users.Count();
                var originalPhoneCount = context.Phones.Count();

                Mapper.Map(viewModel, model);

                foreach (var phone in model2.Phones.ToList())
                {
                    // Must detach the entity from the original user
                    context.Entry(phone).State = EntityState.Detached;
                    model.Phones.Add(phone);
                }

                model2.Name = "abc";

                Assert.DoesNotThrow(() => context.Users.Add(model));
                Assert.DoesNotThrow(() => context.SaveChanges());
                Assert.Equal(originalUserCount + 1, context.Users.Count());
                Assert.Equal(originalPhoneCount + 2, context.Phones.Count());
            }
        }

        [Fact]
        public void TwoDifferentDbContexts_ManuallyAddDetachedChildrenAndModifyTrackedEntity_SaveSuccessfully()
        {
            // Simulate a GET request
            UserViewModel viewModel;
            using (var firstContext = new MvcEFTestContext())
            {
                viewModel = firstContext.Users.Where(u => u.Id == TestUserId).Project().To<UserViewModel>().Single();
            }

            // Simulate modification to the viewModel
            viewModel.Name = "Abc";

            // Simulate a POST request
            using (var secondContext = new MvcEFTestContext())
            {
                var oldVersion = secondContext.Users.Single(u => u.Id == viewModel.Id);

                Mapper.Map(viewModel, oldVersion);

                foreach (var phone in oldVersion.Phones.ToList())
                {
                    secondContext.Entry(phone).State = EntityState.Detached;
                    oldVersion.Phones.Add(phone);
                }
            }
        }

        [Fact]
        public void Test()
        {
            using (var context = new MvcEFTestContext())
            {
                var user = context.Users.Find(1);
                var userViewModel = new PhoneViewModel { Id = 1, Name = "Samsung S5" };

                var phoneEntry = context.Entry(user.Phones.ToList()[0]);
                phoneEntry.CurrentValues.SetValues(userViewModel);

                Assert.Equal("Samsung S5", phoneEntry.Property(p => p.Name).CurrentValue);
                Assert.Equal("5.0", phoneEntry.Property(p => p.AndroidVersion).CurrentValue);
            }
        }

        private static IEnumerable<PhoneViewModel> GetPhoneViewModels(bool modified)
        {
            var unmodifiedPhones = new List<PhoneViewModel>
            {
                new PhoneViewModel { Id = 1, Name = "Nexus 5", AndroidVersion = "4.0" },
                new PhoneViewModel { Id = 2, Name = "Nexus X", AndroidVersion = "5.0" }
            };

            var modifiedPhones = new List<PhoneViewModel>
            {
                new PhoneViewModel { Id = 1, Name = "Nexus 5", AndroidVersion = "4.0" },
                new PhoneViewModel { Id = 2, Name = "Samsung S4", AndroidVersion = "4.0" }
            };

            return modified ? modifiedPhones : unmodifiedPhones;
        }

        private static UserViewModel GetUserViewModel(bool modified, bool childrenModified)
        {
            var unmodifiedUser = new UserViewModel
            {
                Id = TestUserId,
                Name = "Foo",
            };

            var modifiedUser = new UserViewModel
            {
                Id = TestUserId,
                Name = "Foo234",
            };

            unmodifiedUser.Phones = GetPhoneViewModels(childrenModified);
            modifiedUser.Phones = GetPhoneViewModels(childrenModified);

            return modified ? modifiedUser : unmodifiedUser;
        }
    }
}
