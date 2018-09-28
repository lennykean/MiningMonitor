import { MinerNamePipe } from './miner-name.pipe';

describe('MinerNamePipe', () => {
    it('create an instance', () => {
        const pipe = new MinerNamePipe(null);
        expect(pipe).toBeTruthy();
    });
});
