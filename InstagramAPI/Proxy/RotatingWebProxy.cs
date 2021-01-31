using System;
using System.Net;
using System.Collections.Generic;

using InstagramAPI.Models;

namespace InstagramAPI.Proxy {
  public class RotatingWebProxy : IWebProxy {
    private const string PROXY_CHECK_API = "https://api.ipify.org";
    private string myIp;

    private List<ProxyModel> proxies;
    private int currentProxyIndex = 0;

    public ICredentials Credentials { get; set; }
    
    public RotatingWebProxy() {
      proxies = new List<ProxyModel>();
      myIp = GetIP();
    }

    public bool AddProxy(ProxyModel proxy) {
      if(IsProxyWorking(proxy)) {
        Console.WriteLine("Proxy working.");
        proxies.Add(proxy);
        return true;
      }
      Console.WriteLine("Proxy not working.");
      return false;
    }

    public void AddMultipleProxies(ProxyModel[] proxies) {
      for(int i = 0; i < proxies.Length; i++) {
        AddProxy(proxies[i]);
      }
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
    
    private bool IsProxyWorking(ProxyModel proxy) {
      return !(GetIP() != myIp);
    }

    private string GetIP(ProxyModel proxy = null) {
      WebClient wc = new WebClient();
      if(proxy != null) {
        wc.Proxy = new WebProxy(proxy.ipAddress, proxy.port);
      }
      return wc.DownloadString(PROXY_CHECK_API);
    }

    public bool IsBypassed(Uri host) {
      if(proxies.Count <= 0) {
        return true;
      }
      return false;
    }
  }
}