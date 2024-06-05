import { useGigaInhouseApi } from "@giga-inhouse/hooks/use-giga-inhouse-api";
import { useMutation, useQueryClient } from "@tanstack/react-query";

type MutateUserOptions = {
  userName?: string;
  steamId?: string;
};

export function useGigaInhouseMutateUser() {
  const api = useGigaInhouseApi();
  const queryClient = useQueryClient();

  const mutateUser = useMutation({
    mutationFn: async ({ userName, steamId }: MutateUserOptions) => {
      api.request({
        method: "POST",
        url: "/Profile",
        data: {
          userName,
          steamId,
        },
      });
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ["giga-inhouse", "user"] });
    },
  });

  return mutateUser;
}
