import { useAuthContext } from "@giga-inhouse/components/auth-wrapper/use-auth-context";
import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useQuery, keepPreviousData } from "@tanstack/react-query";
import { AxiosError } from "axios";

export type LobbyUser = {
  userId: string;
  userName?: string;
  steamId: string;
};

export type Lobby = {
  lobbyId: string;
  started: boolean;
  users: LobbyUser[];
  isJoined: boolean;
};

type NoSteamIdError = {
  type: "NoSteamId";
  description: string;
};

type GetLobbyError = NoSteamIdError;

export function useGigaInhouseLobby(lobbyId: string | undefined) {
  const api = useGigaInhouseApi();
  const { user } = useAuthContext();

  // Effectively doing long polling here
  const query = useQuery<Lobby, AxiosError<GetLobbyError>>({
    queryKey: ["giga-inhouse", "lobby", lobbyId],
    queryFn: async () => {
      const res = await api.request<Lobby>({
        url: "Lobby",
        method: "GET",
        params: {
          lobbyId,
        },
      });

      return res.data;
    },
    select: (lobby) => {
      const isInLobby = lobby.users.some((lobbyUser) => {
        return lobbyUser.userId === user.userId;
      });
      lobby.isJoined = isInLobby;

      return lobby;
    },
    retry: false,
    placeholderData: keepPreviousData,
    refetchInterval: 5000,
    refetchIntervalInBackground: true,
  });

  return query;
}
