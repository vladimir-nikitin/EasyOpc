
import { createOpcApi } from "./opcApi";
import { createSettingApi } from "./settingApi";
import { createWorkApi } from "./workApi";

export const createApi = (fetcher: typeof fetch) => (
    {
      opc: createOpcApi(fetcher),
      work: createWorkApi(fetcher),
      setting: createSettingApi(fetcher),
    }
  );

export type Api = ReturnType<typeof createApi>;

export const api = createApi(fetch);