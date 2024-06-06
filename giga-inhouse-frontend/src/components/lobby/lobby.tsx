import { LobbyPlayer } from "@giga-inhouse/components/lobby/lobby-player";
import {
  CSTeam,
  GigaInhouseLobby,
} from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import {
  Box,
  Button,
  Center,
  Container,
  Flex,
  Grid,
  Space,
  Stack,
  Text,
  Title,
  useMantineTheme,
} from "@mantine/core";

type LobbyProps = {
  lobby: GigaInhouseLobby;
};

export function Lobby({ lobby }: LobbyProps) {
  const tUsers = lobby.users.filter((user) => {
    return user.team === CSTeam.T;
  });

  const ctUsers = lobby.users.filter((user) => {
    return user.team === CSTeam.CT;
  });

  const theme = useMantineTheme();

  return (
    <Container fluid>
      <Grid grow>
        <Grid.Col span={12}>
          <Center>
            <Button>Shuffle</Button>
          </Center>
        </Grid.Col>
        <Grid.Col span={6}>
          <Stack gap="md">
            <Center>
              <Title size="md">CT</Title>
            </Center>
            {ctUsers.map((user) => {
              return <LobbyPlayer user={user} key={user.userId} />;
            })}
          </Stack>
        </Grid.Col>
        <Grid.Col span={6}>
          <Stack gap="md">
            <Center>
              <Title size="md">T</Title>
            </Center>
            {tUsers.map((user) => {
              return <LobbyPlayer user={user} key={user.userId} />;
            })}
          </Stack>
        </Grid.Col>
        <Grid.Col>
          <Center>
            <Button color="green">Start</Button>
          </Center>
        </Grid.Col>
      </Grid>
    </Container>
  );
}
