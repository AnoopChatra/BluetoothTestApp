using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BluetoothTestApp.Core.Services;
using BluetoothTestApp.Models;

namespace BluetoothTestApp.Services
{
    public class RestQueueService
    {
        private readonly object _lockObject;
        private readonly RestService _restService;

        private LinkedList<ContactDetail> _contactDetailsLinkedList;
  
        public RestQueueService()
        {
            _lockObject = new object();
            _restService = new RestService();
            _contactDetailsLinkedList = new LinkedList<ContactDetail>();
        }

        public void EnqueueContactDetail(ContactDetail contactDetail)
        {
            lock (_lockObject)
            {
                if (0 == _contactDetailsLinkedList.Count)
                {
                    _contactDetailsLinkedList.AddFirst(contactDetail);
                    Task.Factory.StartNew(ProcessQueue);
                }
                else
                {
                    _contactDetailsLinkedList.AddLast(contactDetail);
                }
            }
        }       

        private async Task ProcessQueue()
        {
            ContactDetail contactDetail = DequeueContactDetail();

            while(null != contactDetail)
            {
                await _restService.SaveContactDetailAsync(contactDetail);              
                contactDetail = DequeueContactDetail();
            }
        }

        private ContactDetail DequeueContactDetail()
        {
            ContactDetail contactDetail = null;
            lock (_lockObject)
            {               
                if (_contactDetailsLinkedList.Count > 0)
                {
                    contactDetail = _contactDetailsLinkedList.First.Value;
                   _contactDetailsLinkedList.RemoveFirst();
                }
            }
            return contactDetail;
        }
    }
}
