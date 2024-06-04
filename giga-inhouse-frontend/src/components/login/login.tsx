import React from "react";
import { useNavigate } from "react-router";
import { Link } from "react-router-dom";

export function Login() {
  const [userName, setUserName] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [error, setError] = React.useState("");
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
      .then((res) => {
        if (res.status !== 200) {
          res.text().then((value) => {
            setError(value);
          });
          return;
        }
        const sessionToken = res.headers.get("Session-Token");
        if (!sessionToken) {
          setError("Bad");
          return;
        }
        localStorage.setItem("session-token", sessionToken);

        navigate("/app");
      })
      .catch((err) => {
        setError(err.message);
      });
  };

  return (
    <form onSubmit={handleLogin}>
      <label>
        Username
        <input
          type="text"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
        />
      </label>
      <label>
        password
        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
      </label>
      {error ?? <label>{error}</label>}
      <button>Login</button>
      <Link to="/register">Register</Link>
    </form>
  );
}
