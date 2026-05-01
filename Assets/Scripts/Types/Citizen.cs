using System;

namespace Library
{
    public class Citizen
    {
        private bool _isWorking;
        private Guid _workplace;
        private Guid _residence;
        private Guid _id;
        
        public Citizen(Guid residence)
        {
            _residence = residence;
            _isWorking = false;
            _id = Guid.NewGuid();
            
            ResidentManager.GetResidence(residence).AddResident(_id);
            CitizenManager.Citizens.Add(_id, this);
        }

        public void Leave()
        {
            CitizenManager.Citizens.Remove(_id);
            ResidentManager.GetResidence(_residence).RemoveResident(_id);
            if (_isWorking) WorkManager.GetWorkplace(_workplace).RemoveWorker(_id);
        }

        public void SetWorking(bool work)
        {
            _isWorking = work;
            if (work)
            {
                CitizenManager.JoblessCitizens.Remove(_id);
            }
            else
            {
                CitizenManager.JoblessCitizens.Add(_residence);
            }
        }

        public bool IsWorking()
        {
            return _isWorking;
        }

        public Guid GetResidence()
        {
            return _residence;
        }
    }
}