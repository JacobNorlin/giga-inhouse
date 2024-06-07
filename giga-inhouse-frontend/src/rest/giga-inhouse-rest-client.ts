import axios from "axios";

export const client = axios.create({
  baseURL: "http://localhost:5104",
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});
