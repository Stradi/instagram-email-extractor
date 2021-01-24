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
      return new Uri(GetAvailableProxy());
    }

    // Added this function because in future we may want to choose proxy with another algorithm
    // such as checking request count made by proxy to avoid IP bans etc.
    private string GetAvailableProxy() {
      string proxy = string.Format(
        "http://{0}:{1}",
        proxies[currentProxyIndex].ipAddress,
        proxies[currentProxyIndex].port
      );

      currentProxyIndex++;
      if(currentProxyIndex >= proxies.Count) {
        currentProxyIndex = 0;
      }

      return proxy;
    }

    public bool IsBypassed(Uri host) {
      return false;
    }
  }
}