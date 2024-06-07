import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useCreateGigaInhouseLobby } from "@giga-inhouse/hooks/use-create-giga-inhouse-lobby";
import { useJoinGigaInhouseLobby } from "@giga-inhouse/hooks/use-join-giga-inhouse-lobby";
import { useListGigaInhouseLobbies } from "@giga-inhouse/hooks/use-list-giga-inhouse-lobbies";
import {
  Button,
  Container,
  Flex,
  Grid,
  List,
  Stack,
  Text,
} from "@mantine/core";

export function LobbyListPage() {
  const lobbiesQuery = useListGigaInhouseLobbies();
  const createLobby = useCreateGigaInhouseLobby();
  const joinLobby = useJoinGigaInhouseLobby();

  return (
    <Container size={"xl"}>
      <Grid grow>
        <Grid.Col span={12}>
          <Button
            onClick={() => {
              createLobby.mutate();
            }}
          >
            Create lobby
          </Button>
        </Grid.Col>
        <Grid.Col span={12}>
          <Stack gap="sm">
            <QueryWrapper queryStates={[lobbiesQuery]}>
              {(lobbies) => {
                return lobbies.map((lobby) => {
                  return (
                    <Flex key={lobby.lobbyId} gap="sm" align="center">
                      <Text size="md">{lobby.lobbyId}</Text>
                      <Text size="sm">({lobby.users.length} / 10)</Text>
                      <Button
                        onClick={() =>
                          joinLobby.mutate({ lobbyId: lobby.lobbyId })
                        }
                      >
                        Join
                      </Button>
                    </Flex>
                  );
                });
              }}
            </QueryWrapper>
          </Stack>
        </Grid.Col>
      </Grid>
    </Container>
  );
}
