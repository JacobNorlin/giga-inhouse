import React from "react";
import { useNavigate } from "react-router";

export function Register() {
  const [userName, setUserName] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [error, setError] = React.useState("");
  const navigate = useNavigate();

  const handleRegister = (event: React.FormEvent) => {
    event.preventDefault();

    fetch("http://localhost:5104/User/register", {
      method: "POST",
      body: JSON.stringify({
        userId: userName,
        password: password,
      }),
      headers: {
        "Content-Type": "application/json",
      },
    }).then((res) => {
      if (res.status !== 200) {
        res.text().then((value) => {
          setError(value);
        });
        return;
      }

      navigate("/login");
    });
  };

  return (
    <form onSubmit={handleRegister}>
      <label>
        Username
        <input value={userName} onChange={(e) => setUserName(e.target.value)} />
      </label>
      <label>
        Password
        <input value={password} onChange={(e) => setPassword(e.target.value)} />
      </label>
      {error ?? <p>{error}</p>}
      <button>Register</button>
    </form>
  );
}
