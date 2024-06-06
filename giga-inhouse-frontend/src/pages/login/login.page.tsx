import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";
import {
  Alert,
  Button,
  Center,
  Input,
  InputLabel,
  Stack,
  Text,
} from "@mantine/core";
import { AxiosError, AxiosHeaders } from "axios";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";

type BadLoginError = {
  type: "BadLogin";
  description: string;
};

type UnknownLoginError = {
  type: "Unknown";
  description: string;
};

type LoginError = BadLoginError | UnknownLoginError;

function getErrorTitle(error: LoginError) {
  switch (error.type) {
    case "BadLogin":
      return "Failed to login";
  }

  return "Something went wrong";
}

export function Login() {
  const [userName, setUserName] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [error, setError] = React.useState<LoginError | undefined>();
  const navigate = useNavigate();
  const api = useGigaInhouseApi();

  const handleLogin = async (event: React.FormEvent) => {
    event.preventDefault();
    try {
      const res = await api.request({
        url: "/User/login",
        method: "POST",
        data: {
          userId: userName,
          providedPassword: password,
        },
      });

      if (res.headers instanceof AxiosHeaders) {
        const sessionToken = res.headers.get("Session-Token")?.toString();
        if (sessionToken) {
          localStorage.setItem("session-token", sessionToken);
          navigate("/");
        } else {
          setError({
            type: "Unknown",
            description: "No session token",
          });
        }
      } else {
        setError({
          type: "Unknown",
          description: "No session token",
        });
      }
    } catch (err) {
      if (err instanceof AxiosError) {
        const error = err.response?.data as LoginError;
        setError(error);
      }
    }
  };

  return (
    <Stack gap="sm">
      <Center>
        <Text size="lg">Login</Text>
      </Center>
      <InputLabel>
        Username
        <Input value={userName} onChange={(e) => setUserName(e.target.value)} />
      </InputLabel>
      <InputLabel>
        Password
        <Input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </InputLabel>
      {error ? (
        <Alert title={getErrorTitle(error)}>{error.description}</Alert>
      ) : null}

      <Stack>
        <Button onClick={handleLogin}>Login</Button>
        <Link to="register">
          <Button>Register</Button>
        </Link>
      </Stack>
    </Stack>
  );
}
