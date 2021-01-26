using System.Collections.Generic;

namespace InstagramAPI.Requests {
  public enum RequestType {
    GET,
    POST
  }

  public struct RequestParams {
    public string name;
    public string value;
  }

  public abstract class BaseRequest {
    private protected string url;
    private protected RequestParams[] parameters;
    private string finalUrl;

    public RequestType requestType;
     
    public string URL { get { return url; } }

    public List<RequestHeader> additionalHeaders;
     
    public BaseRequest(string url, RequestType requestType, params RequestParams[] parameters) {
      this.url = url;
      this.requestType = requestType;
      this.parameters = parameters;
      this.finalUrl = "";

      this.additionalHeaders = new List<RequestHeader>();
    }

    public override string ToString() {
      if(!string.IsNullOrWhiteSpace(finalUrl)) {
        return finalUrl;
      }

      if(parameters.Length == 0) {
        finalUrl = url;
        return url;
      }

      finalUrl = url;
      for(int i = 0; i < parameters.Length; i++) {
        RequestParams parameter = parameters[i];
        if(i == 0) {
          finalUrl += "?";
        }
        finalUrl += string.Format("{0}={1}", parameter.name, parameter.value);
      
        if(i != parameters.Length - 1) {
          finalUrl += "&";
        }
      }
      
      return finalUrl;
    }
  }
}