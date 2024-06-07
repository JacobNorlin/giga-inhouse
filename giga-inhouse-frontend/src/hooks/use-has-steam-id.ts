import { useAuthContext } from "@giga-inhouse/components/auth-wrapper/use-auth-context";

// Helper hook for gating stuff that requires steam id
export function useHasSteamId() {
  const { user } = useAuthContext();

  return !!user.steamId;
}
