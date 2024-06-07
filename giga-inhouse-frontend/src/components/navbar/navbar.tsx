import { NavLink } from "@mantine/core";
import { NavLink as RouterNavLink } from "react-router-dom";

export function NavBar() {
  return (
    <nav>
      <NavLink to="profile" label="Profile" component={RouterNavLink}></NavLink>
      <NavLink to="lobbies" label="Lobbies" component={RouterNavLink}></NavLink>
    </nav>
  );
}
