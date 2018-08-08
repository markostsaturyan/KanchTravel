﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Kanch.ProfileComponents.DataModel;

namespace Kanch.ProfileComponents.HelperClasses
{
    class UnconfirmedTrips
    {
        public CampingTripInfo CampingTrip { get; set; }
        public ICommand ConfirmCommand { get; set; }
        public ICommand IgnoreCommand { get; set; }
    }
}