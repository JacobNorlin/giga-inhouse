import { NavLink, Stack } from "@mantine/core";
import { NavLink as RouterNavLink } from "react-router-dom";

export function NavBar() {
  return (
    <nav>
      <NavLink to="profile" label="Profile" component={RouterNavLink}></NavLink>
      <NavLink to="lobby" label="Lobby" component={RouterNavLink}></NavLink>
    </nav>
  );
}
