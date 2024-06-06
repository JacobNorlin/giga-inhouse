import { Lobby } from "@giga-inhouse/components/lobby/lobby";
import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useGigaInhouseLobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { Alert } from "@mantine/core";

export function LobbyPage() {
  const lobbyQuery = useGigaInhouseLobby();

  return (
    <>
      Hello
      <QueryWrapper
        queryStates={[lobbyQuery]}
        renderError={(lobbyError) => {
          const error = lobbyError.response?.data;
          return <Alert title={error?.type}>{error?.description}</Alert>;
        }}
      >
        {(lobby) => {
          return <Lobby lobby={lobby} />;
        }}
      </QueryWrapper>
    </>
  );
}
