export interface Alert {
    id: string;
    minerId: string;
    alertDefinitionId: string;
    message: string;
    start: Date;
    lastActive: Date;
    end: Date;
    acknowledgedAt: Date;
    acknowledged: boolean;
    active: boolean;
}
