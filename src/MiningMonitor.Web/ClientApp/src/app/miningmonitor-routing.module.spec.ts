import { MiningMonitorRoutingModule } from './miningmonitor-routing.module';

describe('MiningmonitorRoutingModule', () => {
    let miningMonitorRoutingModule: MiningMonitorRoutingModule;

    beforeEach(() => {
        miningMonitorRoutingModule = new MiningMonitorRoutingModule();
    });

    it('should create an instance', () => {
        expect(miningMonitorRoutingModule).toBeTruthy();
    });
});
