using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using InstagramAPI.Models;
using System;

namespace InstagramAPI.Proxy {
  public class RotatingWebProxy : IWebProxy {
    private List<ProxyModel> proxies;
    private int currentProxyIndex = 0;

    public ICredentials Credentials { get; set; }
    
    public RotatingWebProxy(List<ProxyModel> proxies) {
      this.proxies = proxies;
    }

    public Uri GetProxy(Uri destination) {
      string proxy = string.Format(
        "http://{0}:{1}",
        proxies[currentProxyIndex].ipAddress,
        proxies[currentProxyIndex].port
      );

      currentProxyIndex++;
      if(currentProxyIndex >= proxies.Count) {
        currentProxyIndex = 0;
      }
      
      return new Uri(proxy);
    }

    public bool IsBypassed(Uri host) {
      return false;
    }
  }
}