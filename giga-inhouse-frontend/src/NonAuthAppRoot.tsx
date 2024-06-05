import { Header } from "@giga-inhouse/components/header/header";
import { AppShell, Stack } from "@mantine/core";
import { Outlet } from "react-router";

export function NonAuthAppRoot() {
  return (
    <AppShell header={{ height: 60 }}>
      <AppShell.Header>
        <Header />
      </AppShell.Header>
      <AppShell.Main>
        <Stack align="center" flex={1}>
          <Outlet />
        </Stack>
      </AppShell.Main>
    </AppShell>
  );
}
