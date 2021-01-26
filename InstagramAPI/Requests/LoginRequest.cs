using System.Collections.Generic;
using System.Net.Http;

using InstagramAPI.Models;
using InstagramAPI.Helpers;

namespace InstagramAPI.Requests {
  public struct LoginRequestParams {
    public CredentialModel credential;
    public RequestHeader rolloutHash;
    public PasswordEncryptionModel passwordEncryptionValues;
  }

  public class LoginRequest : PostRequest {
    LoginRequestParams loginRequestParams;
    string encryptedPassword = "";

    public LoginRequest(LoginRequestParams loginRequestParams, string url, params RequestParams[] parameters)
    : base(url, null, parameters) {
      this.loginRequestParams = loginRequestParams;
      this.additionalHeaders.Add(this.loginRequestParams.rolloutHash);
    }

    public override HttpContent GetData() {
      return new FormUrlEncodedContent(new [] {
        new KeyValuePair<string, string>("username", GetUsername()),
        new KeyValuePair<string, string>("enc_password", GetEncryptedPassword())
      });
    }

    public string GetUsername() {
      return loginRequestParams.credential.username;
    }

    public string GetEncryptedPassword() {
      if(string.IsNullOrWhiteSpace(encryptedPassword)) {
        encryptedPassword = PasswordHelper.GenerateEncPassword(
          loginRequestParams.credential.password,
          loginRequestParams.passwordEncryptionValues.publicKey,
          loginRequestParams.passwordEncryptionValues.keyId,
          loginRequestParams.passwordEncryptionValues.keyVersion
        );
      }

      return encryptedPassword;
    }
  }
}