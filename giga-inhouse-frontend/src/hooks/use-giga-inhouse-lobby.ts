import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useQuery, keepPreviousData } from "@tanstack/react-query";
import { AxiosError } from "axios";

export enum CSTeam {
  T = 0,
  CT = 1,
}

export type LobbyUser = {
  userId: string;
  userName?: string;
  team: CSTeam;
  steamId: string;
};

export type GigaInhouseLobby = {
  users: LobbyUser[];
};

type NoSteamIdError = {
  type: "NoSteamId";
  description: string;
};

type GetLobbyError = NoSteamIdError;

export function useGigaInhouseLobby() {
  const api = useGigaInhouseApi();

  // Effectively doing long polling here
  const query = useQuery<GigaInhouseLobby, AxiosError<GetLobbyError>>({
    queryKey: ["giga-inhouse", "lobby"],
    queryFn: async () => {
      const res = await api.request<GigaInhouseLobby>({
        url: "Lobby",
        method: "GET",
      });

      return res.data;
    },
    retry: false,
    placeholderData: keepPreviousData,
    refetchInterval: 1000,
    refetchIntervalInBackground: true,
  });

  return query;
}
