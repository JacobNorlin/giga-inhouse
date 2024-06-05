import { QueryWrapper } from "@giga-inhouse/components/query-wrapper/query-wrapper";
import { useGigaInhouseLobby } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";

export function Lobby() {
  const lobbyQuery = useGigaInhouseLobby();

  return (
    <>
    Hello
    <QueryWrapper queryStates={[lobbyQuery]}>
      {(lobby) => {
        return (
          <div>
            <div>
              <label>Users</label>
              {lobby.users.map((user) => {
                return <label key={user.userId}>{user.name}</label>;
              })}
            </div>
          </div>
        );
      }}
    </QueryWrapper>
    </>
  );
}
