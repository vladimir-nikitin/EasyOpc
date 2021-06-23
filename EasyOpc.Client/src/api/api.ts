import { createOpcDaApi } from "./opcDaApi";
import { createOpcUaApi } from "./opcUaApi";
import { createSettingsApi } from "./settingsApi";
import { createWorksApi } from "./worksApi";

export const createApi = (fetcher: typeof fetch) => (
    {
      opcDa: createOpcDaApi(fetcher),
      opcUa: createOpcUaApi(fetcher),
      settings: createSettingsApi(fetcher),
      works: createWorksApi(fetcher),
    }
  );

export type Api = ReturnType<typeof createApi>;

export const api = createApi(fetch);