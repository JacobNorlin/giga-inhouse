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
import { LobbyPage } from "@giga-inhouse/pages/lobby/lobby.page";
import "@mantine/core/styles.css";
import { ProfilePage } from "@giga-inhouse/pages/profile/profile.page";
import { LobbyListPage } from "@giga-inhouse/pages/lobby-list/lobby-list.page";

const router = createBrowserRouter(
  [
    {
      path: "/",
      element: <AppRoot />,
      children: [
        {
          path: "lobbies",
          element: <LobbyListPage />,
        },
        {
          path: "lobby/:lobbyId",
          element: <LobbyPage />,
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
  ],
  {
    basename: "/inhouse",
  }
);

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
