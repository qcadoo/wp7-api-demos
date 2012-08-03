﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Com.Mobeelizer.Mobile.Wp7;
using wp7_api_demos.Model.MobeelizerModels;

namespace wp7_api_demos.ViewModel
{
    public class SelectScorePageViewModel : ViewModelBase
    {
        private String modelGuid;

        public ObservableCollection<int> Options { get; private set; }

        public SelectScorePageViewModel(INavigationService navigationService, String modelGuid)
            : base(navigationService)
        {
            this.modelGuid = modelGuid;
            this.Options = new ObservableCollection<int>();
            for(int i = 1; i<6; ++i)
            {
                this.Options.Add(i);
            }
        }

        public int SelectedOption
        {
            get
            {
                return -1;
            }

            set
            {
                using (var transaction = Mobeelizer.GetDatabase().BeginTransaction())
                {
                    var query = from conflictsEntity e in transaction.GetModels<conflictsEntity>() where e.guid == modelGuid select e;
                    conflictsEntity entity = query.Single();
                    entity.score = value;
                    transaction.Commit();
                }
                this.navigationService.GoBack();
            }
        }
    }
}