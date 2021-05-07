using EasyOpc.WinService.Core.Logger.Contract;
using EasyOpc.WinService.Modules.Opc.Connectors.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyOpc.WinService.Modules.Opc.Connectors
{
    public class OpcGroup : IOpcGroup
    {
        private event OpcItemsChanged _opcItemsChanged;

        private ILogger Logger { get; }

        private object LockObject { get; } = new object();

        private IOpcServer OpcServer { get; }

        private IDictionary<string, IOpcItem> OpcItems { get; }

        private Func<IOpcItem, string> OpcItemKeySelector { get; }

        private int Listners { get; set; }

        public Guid Id { get; }

        public string Name { get; }

        public event OpcItemsChanged OpcItemsChanged
        {
            add
            {
                lock(LockObject)
                {
                    _opcItemsChanged += value;
                    Listners++;
                    Logger.Debug($"[{nameof(OpcGroup)}][{Name}][Event][{nameof(OpcItemsChanged)}] listner added [Listners count: {Listners}]");
                }
            }
            remove
            {
                lock (LockObject)
                {
                    _opcItemsChanged -= value;
                    Listners--;
                    Logger.Debug($"[{nameof(OpcGroup)}][{Name}][Event][{nameof(OpcItemsChanged)}] listner removed [Listners count: {Listners}]");

                    if (Listners <= 0)
                    {
                        Logger.Debug($"[{nameof(OpcGroup)}][{Name}][Event][{nameof(OpcItemsChanged)}] Listners <= 0. Remove group from OpcServer.");
                        OpcServer.RemoveOpcGroupAsync(Id).GetAwaiter().GetResult();
                    }
                }
            }
        }

        public OpcGroup(ILogger logger, IOpcServer opcServer, Guid id, string name, IEnumerable<IOpcItem> opcItems, Func<IOpcItem, string> opcItemKeySelector)
        {
            Logger = logger;
            OpcServer = opcServer;
            Id = id;
            Name = name;
            OpcItems = opcItems.ToDictionary(opcItemKeySelector, p => p);
            OpcItemKeySelector = opcItemKeySelector;
        }

        public void CallOpcItemsChangedEvent(IEnumerable<IOpcItem> items)
        {
            lock(LockObject)
            {
                if(_opcItemsChanged != null)
                {
                    _opcItemsChanged(items);
                }
            }

            lock(OpcItems)
            {
                IOpcItem current;
                foreach (var item in items)
                {
                    current = OpcItems[OpcItemKeySelector(item)];
                    if (current == null) continue;

                    current.Quality = string.IsNullOrEmpty(item.Quality) ? current.Quality : item.Quality;
                    current.ReadOnly = item.ReadOnly;
                    current.ReqDataType = string.IsNullOrEmpty(item.ReqDataType) ? current.ReqDataType : item.ReqDataType;
                    current.TimestampLocal = string.IsNullOrEmpty(item.TimestampLocal) ? current.TimestampLocal : item.TimestampLocal;
                    current.TimestampUtc = string.IsNullOrEmpty(item.TimestampUtc) ? current.TimestampUtc : item.TimestampUtc;
                    current.Value = string.IsNullOrEmpty(item.Value) ? current.Value : item.Value;
                }
            }
        }

        public IDictionary<string, IOpcItem> GetOpcItems() => OpcItems;
    }
}
