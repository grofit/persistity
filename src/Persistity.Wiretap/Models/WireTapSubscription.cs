using System;

namespace Persistity.Wiretap.Models
{
    public class WireTapSubscription
    {
        private readonly Action _unsubscribeAction;
        
        public WireTapSubscription(Action unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
        }

        public void Unsubscribe() => _unsubscribeAction();
    }
}