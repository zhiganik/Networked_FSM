﻿namespace Fusion.Addons.KCC
{
    public partial class KCCData
    {
        // PUBLIC MEMBERS

        public bool Sprint;
        public bool Crouch;
        public float Stamina = 100f;

        // PARTIAL METHODS

        partial void CopyUserDataFromOther(KCCData other)
        {
            // Making a deep copy for correct rollback from local history.
            // This method is executed when you get a new state from server.
            // It is also executed after fixed updates to copy fixed data to render data.

            Sprint = other.Sprint;
            Crouch = other.Crouch;
            Stamina = other.Stamina;

            // Because this method is partial and cannot be implemented for each property separately, you have to copy all properties here.
            // Or use this method as a wrapper and split execution into multiple calls.
        }
    }
}