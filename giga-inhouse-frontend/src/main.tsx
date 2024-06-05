import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { DEFAULT_THEME, MantineProvider } from "@mantine/core";
import { AppRoot } from "@giga-inhouse/AppRoot";
import { NonAuthAppRoot } from "@giga-inhouse/NonAuthAppRoot";
import { RegisterPage } from "@giga-inhouse/pages/register/register.page.";
import { Login } from "@giga-inhouse/pages/login/login.page";
import { Lobby } from "@giga-inhouse/pages/lobby/lobby.page";
import "@mantine/core/styles.css";
import { ProfilePage } from "@giga-inhouse/pages/profile/profile.page";

const router = createBrowserRouter([
  {
    path: "/",
    element: <AppRoot />,
    children: [
      {
        path: "lobby",
        element: <Lobby />,
      },
      {
        path: "profile",
        element: <ProfilePage />,
      },
    ],
  },
  {
    path: "/login",
    element: <NonAuthAppRoot />,
    children: [
      {
        index: true,
        element: <Login />,
      },
      {
        path: "register",
        element: <RegisterPage />,
      },
    ],
  },
]);

const queryClient = new QueryClient();

const theme = DEFAULT_THEME;

ReactDOM.createRoot(document.getElementById("root")!).render(
  <React.StrictMode>
    <MantineProvider theme={theme}>
      <QueryClientProvider client={queryClient}>
        <RouterProvider router={router} />
      </QueryClientProvider>
    </MantineProvider>
  </React.StrictMode>
);
