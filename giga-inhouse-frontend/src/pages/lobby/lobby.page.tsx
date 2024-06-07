import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useGigaInhouseLobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { useJoinGigaInhouseLobby } from "@giga-inhouse/hooks/use-join-giga-inhouse-lobby";
import { Anchor, Button, Group, Stack, Text } from "@mantine/core";
import { useParams } from "react-router";

export function LobbyPage() {
  const { lobbyId } = useParams<{ lobbyId: string }>();
  const lobbyQuery = useGigaInhouseLobby(lobbyId);
  const joinLobby = useJoinGigaInhouseLobby();

  return (
    <QueryWrapper queryStates={[lobbyQuery]}>
      {(lobby) => {
        return (
          <Stack gap="lg">
            <Group>
              <Text size="lg">{lobbyId}</Text>
              {!lobby.isJoined ? (
                <Button onClick={() => joinLobby.mutate({ lobbyId })}>
                  Join
                </Button>
              ) : null}
            </Group>

            <Stack gap="md">
              <Stack gap="md">
                {lobby.users.map((user) => {
                  return (
                    <Group key={user.userId}>
                      <Text size="sm">{user.userName ?? user.userId}</Text>
                    </Group>
                  );
                })}
              </Stack>
              <Anchor href="steam://connect/192.168.1.127">
                Connect
              </Anchor>
            </Stack>
          </Stack>
        );
      }}
    </QueryWrapper>
  );
}
