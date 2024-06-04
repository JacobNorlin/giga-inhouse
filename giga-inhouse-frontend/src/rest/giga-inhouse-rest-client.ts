import { tokenProvider } from "@giga-inhouse/rest/session-token-provider";
import axios from "axios";

export const client = axios.create({
  baseURL: "http://localhost:5104",
  headers: {
    "Content-Type": "application/json",
  },
});

client.interceptors.request.use((request) => {
  request.headers.set("session-token", tokenProvider.getSessionToken());

  return request;
});
