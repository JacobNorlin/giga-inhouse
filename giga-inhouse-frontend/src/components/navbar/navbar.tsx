import { useHasSteamId } from "@giga-inhouse/hooks/use-has-steam-id";
import { NavLink } from "@mantine/core";
import { ExclamationMark } from "@phosphor-icons/react";
import { NavLink as RouterNavLink } from "react-router-dom";

export function NavBar() {
  const hasSteamId = useHasSteamId();
  return (
    <nav>
      <NavLink
        to="profile"
        label="Profile"
        component={RouterNavLink}
        rightSection={!hasSteamId ? <ExclamationMark color="red" /> : null}
      ></NavLink>
      <NavLink
        to="lobbies"
        label="Lobbies"
        component={RouterNavLink}
        disabled={!hasSteamId}
      ></NavLink>
    </nav>
  );
}
