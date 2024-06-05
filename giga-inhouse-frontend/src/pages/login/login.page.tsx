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
import { AxiosError } from "axios";

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

function parseError(err: string): LoginError {
  const parsed = JSON.parse(err);
  return parsed as LoginError;
}

export function Login() {
  const [userName, setUserName] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [error, setError] = React.useState<LoginError | undefined>();
  const navigate = useNavigate();

  const handleLogin = (event: React.FormEvent) => {
    event.preventDefault();
    fetch("http://localhost:5104/User/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        userId: userName,
        providedPassword: password,
      }),
    })
      .then(async (res) => {
        if (res.status !== 200) {
          const error = (await res.json()) as LoginError;
          setError(error);
          return;
        }
        const sessionToken = res.headers.get("Session-Token");
        if (!sessionToken) {
          setError({
            type: "Unknown",
            description: "Could not find token",
          });
          return;
        }
        localStorage.setItem("session-token", sessionToken);

        navigate("/");
      })
      .catch((err: AxiosError) => {
        setError({
          type: "Unknown",
          description: err.message,
        });
      });
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
