export class SessionTokenProvider {
  getSessionToken() {
    return localStorage.getItem("session-token");
  }
}

export const tokenProvider = new SessionTokenProvider();
