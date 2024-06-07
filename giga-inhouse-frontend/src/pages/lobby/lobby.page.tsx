import { Lobby } from "@giga-inhouse/components/lobby/lobby";
import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useGigaInhouseLobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { useParams } from "react-router";

export function LobbyPage() {
  const { lobbyId } = useParams<{ lobbyId: string }>();
  const lobbyQuery = useGigaInhouseLobby(lobbyId);

  return (
    <QueryWrapper queryStates={[lobbyQuery]}>
      {(lobby) => <Lobby lobby={lobby} />}
    </QueryWrapper>
  );
}
