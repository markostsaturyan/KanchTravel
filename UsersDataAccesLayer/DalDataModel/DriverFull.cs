﻿using System;
using Veldrid.ImageSharp;

namespace UsersDataAccesLayer.DalDataModel
{
    public class DriverFull
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DataOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public ImageSharpTexture Image { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Car Car { get; set; }

        public ImageSharpTexture DrivingLicencePicFront { get; set; }

        public ImageSharpTexture DrivingLicencePicBack { get; set; }

        public string KnowledgeOfLanguages { get; set; }

        public double Raiting { get; set; }
    }
}
