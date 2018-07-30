using System.ComponentModel;
using System.Windows;

namespace Kanch.ViewModel
{
    public class PhotographerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility photographerVisible;
        private string profession;
        private string workExperience;
        private bool hasDron;
        private bool hasCameraStabilizator;
        private bool hasGopro;
        private string cameraModel;
        private bool isProfessional;

        public Visibility PhotographerVisible
        {
            get
            {
                return photographerVisible;
            }

            set
            {
                if (photographerVisible != value)
                {
                    photographerVisible = value;
                    NotifyPropertyChanged("PhotographerVisible");
                }
            }
        }

        public string Profession
        {
            get
            {
                return this.profession;
            }
            set
            {
                if (this.profession != value)
                {
                    this.profession = value;
                    NotifyPropertyChanged("Profession");
                }
            }
        }

        public string WorkExperience
        {
            get
            {
                return this.workExperience;
            }
            set
            {
                if (this.workExperience != value)
                {
                    this.workExperience = value;
                    NotifyPropertyChanged("WorkExperience");
                }
            }
        }

        public bool HasDron
        {
            get
            {
                return this.hasDron;
            }
            set
            {
                if (this.hasDron != value)
                {
                    this.hasDron = value;
                    NotifyPropertyChanged("HasDron");
                }
            }
        }

        public bool HasCameraStabilizator
        {
            get
            {
                return this.hasCameraStabilizator;
            }
            set
            {
                if(this.hasCameraStabilizator != value)
                {
                    this.hasCameraStabilizator = value;
                    NotifyPropertyChanged("HasCameraStabilizator");
                }
            }
        }

        public bool HasGopro
        {
            get
            {
                return this.hasGopro;
            }
            set
            {
                if (this.hasGopro != value)
                {
                    this.hasGopro = value;
                    NotifyPropertyChanged("HasGopro");
                }
            }
        }

        public string CameraModel
        {
            get
            {
                return this.cameraModel;
            }
            set
            {
                if (this.cameraModel != value)
                {
                    this.cameraModel = value;
                    NotifyPropertyChanged("CameraModel");
                }
            }
        }

        public bool IsProfessional
        {
            get
            {
                return this.isProfessional;
            }
            set
            {
                if (this.isProfessional != value)
                {
                    this.isProfessional = value;
                    NotifyPropertyChanged("IsProfessional");
                }
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public PhotographerViewModel()
        {
            this.photographerVisible = Visibility.Collapsed;
        }

        public void Reset()
        {
            this.Profession = "";
            this.WorkExperience = "";
            this.HasDron = false;
            this.HasCameraStabilizator = false;
            this.HasGopro = false;
            this.CameraModel = "";
            this.IsProfessional = false;
        }

        private bool ProfessionValidation(ref string status)
        {
            if (Profession == null)
            {
                status = "Enter your profession.";
                return false;
            }
            return true;
        }

        private bool WorkExperienceValidation(ref string status)
        {
            if (WorkExperience == null)
            {
                status = "Enter your work experience.";
                return false;
            }
            return true;
        }

        private bool CameraModelValidation(ref string status)
        {
            if (CameraModel == null)
            {
                status = "Enter model of the Camera.";
                return false;
            }
            return true;
        }

        public bool PhotographerInfoValidation(out string status)
        {
            status = "";
            if (!ProfessionValidation(ref status))
                return false;
            else if (!WorkExperienceValidation(ref status))
                return false;
            else if (!CameraModelValidation(ref status))
                return false;
            return true;
        }
    }
}
