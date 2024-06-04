import {
  AuthContextProvider,
  User,
} from "@giga-inhouse/components/auth-wrapper/auth-context";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { AxiosError } from "axios";
import React from "react";
import { useNavigate } from "react-router";

type AuthWrapperProps = React.PropsWithChildren;

export function AuthWrapper({ children }: AuthWrapperProps) {
  const navigate = useNavigate();
  const [user, setUser] = React.useState<User | null>(null);

  const api = useGigaInhouseApi();

  React.useEffect(() => {
    api
      .request<User>({
        url: "/User",
        method: "GET",
      })
      .then((res) => {
        setUser(res.data);
      })
      .catch((ex: AxiosError) => {
        if (ex.response?.status === 401) {
          // User not authorized, redirect to login
          navigate("/login");
          return;
        }
      });
  }, []);

  if (!user) {
    return null;
  }

  return <AuthContextProvider user={user}>{children}</AuthContextProvider>;
}
