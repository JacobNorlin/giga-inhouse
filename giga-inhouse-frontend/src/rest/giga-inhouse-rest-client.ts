import axios from "axios";

export const client = axios.create({
  baseURL: import.meta.env.VITE_GIGA_INHOUSE_URL,
  headers: {
    "Content-Type": "application/json",
  },
  withCredentials: true,
});
