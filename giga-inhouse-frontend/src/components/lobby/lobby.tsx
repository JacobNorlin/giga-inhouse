import { LobbyPlayer } from "@giga-inhouse/components/lobby/lobby-player";
import type { Lobby as GigaInhouseLobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { useHasSteamId } from "@giga-inhouse/hooks/use-has-steam-id";
import { useJoinGigaInhouseLobby } from "@giga-inhouse/hooks/use-join-giga-inhouse-lobby";
import { Button, Container, Group, Stack, Text } from "@mantine/core";

type LobbyProps = {
  lobby: GigaInhouseLobby;
};

export function Lobby({ lobby }: LobbyProps) {
  const hasSteamId = useHasSteamId();
  const joinLobby = useJoinGigaInhouseLobby();
  const { lobbyId, users } = lobby;

  return (
    <Container fluid>
      <Stack gap="lg">
        <Group>
          <Text size="lg">{lobbyId}</Text>
          {!lobby.isJoined ? (
            <Button
              onClick={() => joinLobby.mutate({ lobbyId })}
              disabled={!hasSteamId}
            >
              Join
            </Button>
          ) : null}
        </Group>

        <Stack gap="md">
          <Stack gap="md">
            {users.map((user) => {
              return <LobbyPlayer user={user} />;
            })}
          </Stack>
          {/* <Anchor href="steam://connect/192.168.1.127">Connect</Anchor> */}
        </Stack>
      </Stack>
    </Container>
  );
}
