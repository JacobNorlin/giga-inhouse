import React from "react";
import { useNavigate } from "react-router";
import {
  Alert,
  Anchor,
  Button,
  Center,
  Input,
  InputLabel,
  Stack,
  Text,
} from "@mantine/core";
import { Link } from "react-router-dom";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { AxiosError } from "axios";

type UserAlreadyExistsError = {
  type: "UserExists";
  description: string;
};

type MissingCredentialsError = {
  type: "MissingCredentials";
  description: string;
};

type UnknownRegistrationError = {
  type: "Unknown";
  description: string;
};

type RegistrationError =
  | UserAlreadyExistsError
  | MissingCredentialsError
  | UnknownRegistrationError;

function getErrorTitle(error: RegistrationError) {
  switch (error.type) {
    case "MissingCredentials":
      return "Bad request";
    case "UserExists":
      return "User Exists";
  }
  return "Something went wrong";
}

export function RegisterPage() {
  const [userName, setUserName] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [error, setError] = React.useState<RegistrationError | undefined>();
  const navigate = useNavigate();

  const api = useGigaInhouseApi();

  const handleRegister = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      await api.request({
        url: "/User/register",
        method: "POST",
        data: {
          userId: userName,
          password: password,
        },
      });

      navigate("/login");
    } catch (err) {
      if (err instanceof AxiosError) {
        setError(err.response?.data as RegistrationError);
      }
    }
  };

  return (
    <Stack gap="sm">
      <Center>
        <Text size="lg">Create new user</Text>
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
        <Button onClick={handleRegister}>Register</Button>
        <Link to="..">
          <Button>Back</Button>
        </Link>
      </Stack>
    </Stack>
  );
}
