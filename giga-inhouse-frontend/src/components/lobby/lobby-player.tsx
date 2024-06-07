import { LobbyUser } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { Box, Text, useMantineTheme } from "@mantine/core";

type LobbyPlayerProps = {
  user: LobbyUser;
};

export function LobbyPlayer({ user }: LobbyPlayerProps) {
  const theme = useMantineTheme();

  return (
    <Box
      style={{
        overflow: "hidden",
        textOverflow: "ellipsis",
        borderRadius: theme.radius.md,
        background: theme.colors.blue[0],
        padding: theme.spacing.md,
        inlineSize: "100%",
      }}
    >
      <Text size="sm">{user.userName ?? user.userId}</Text>
    </Box>
  );
}
