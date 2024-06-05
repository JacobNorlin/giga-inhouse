import { AuthWrapper } from "@giga-inhouse/components/auth-wrapper/auth-wrapper";
import { Header } from "@giga-inhouse/components/header/header";
import { NavBar } from "@giga-inhouse/components/navbar/navbar";
import { AppShell, Burger, Text } from "@mantine/core";
import { useDisclosure } from "@mantine/hooks";
import { Outlet } from "react-router-dom";

export function AppRoot() {
  const [opened, { toggle }] = useDisclosure();
  return (
    <AuthWrapper>
      <AppShell
        navbar={{
          width: 300,
          breakpoint: "sm",
          collapsed: { mobile: !opened },
        }}
        header={{ height: 60 }}
        padding="md"
      >
        <AppShell.Header>
          <Burger opened={opened} onClick={toggle} hiddenFrom="sm" size="sm" />
          <Header />
        </AppShell.Header>

        <AppShell.Navbar>
          <NavBar />
        </AppShell.Navbar>

        <AppShell.Main>
          <Outlet />
        </AppShell.Main>
      </AppShell>
    </AuthWrapper>
  );
}
