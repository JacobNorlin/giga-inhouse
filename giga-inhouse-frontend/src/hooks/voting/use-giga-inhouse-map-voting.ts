import { useAuthContext } from "@giga-inhouse/components/auth-wrapper/use-auth-context";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { LobbyUser } from "@giga-inhouse/hooks/use-giga-inhouse-lobby";
import { useQuery } from "@tanstack/react-query";

export type MapVote = {
  user: LobbyUser;
  mapName: string;
};

type CsMap = {
  name: string;
  title: string;
  image: string;
};

export type MapVoting = {
  votes: MapVote[];
  bannedMaps: string[];
  maps: CsMap[];
};

export function useGigaInhouseVoting(lobbyId: string | undefined) {
  const api = useGigaInhouseApi();

  const votingQuery = useQuery({
    queryKey: ["giga-inhouse", "voting", "lobbyId"],
    queryFn: async () => {
      const res = await api.request<MapVoting>({
        url: "/Lobby/Vote",
        method: "GET",
        params: {
          lobbyId,
        },
      });

      return res.data;
    },
    select: (mapVoting) => {
      return mapVoting;
    },
    refetchInterval: 1000,
  });

  return votingQuery;
}
