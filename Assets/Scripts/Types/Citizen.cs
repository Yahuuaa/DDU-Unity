using System;

namespace Library
{
    public class Citizen
    {
        private float _income;
        private bool _isWorking;
        
        private Guid _residence;
        private Guid _id;
        
        public Citizen(Guid residence)
        {
            _residence = residence;
            _isWorking = false;
            _income = 0f;
            _id = Guid.NewGuid();
            
            ResidentManager.GetResidence(residence).AddResident(this);
            CitizenManager.Citizens.Add(_id, this);
        }

        public void Leave()
        {
            CityManager.income -= _income;
            CitizenManager.Citizens.Remove(_id);
            ResidentManager.Residence.Remove(_residence);
        }

        public void ChangeIncome(float income)
        {
            _income += income;
            CityManager.income += income;
        }

        public float GetIncome()
        {
            return _income;
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