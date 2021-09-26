import { AlertActionDefinition } from './AlertActionDefinition';

export interface WebHookAlertActionDefinition extends AlertActionDefinition {
  url: string;
  body: string;
}
