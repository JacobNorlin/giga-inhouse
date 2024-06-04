import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import "./index.css";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Login } from "@giga-inhouse/components/login/login.tsx";
import { Register } from "@giga-inhouse/components/register/register.tsx";
import { AuthWrapper } from "@giga-inhouse/components/auth-wrapper/auth-wrapper.tsx";

const router = createBrowserRouter([
  {
    path: "/app",
    element: (
      <AuthWrapper>
        <App />
      </AuthWrapper>
    ),
    children: [],
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/register",
    element: <Register />,
  },
]);

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
